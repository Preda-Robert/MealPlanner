import { Component, computed, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Member } from '../../_models/member';
import {TabDirective, TabsetComponent, TabsModule} from 'ngx-bootstrap/tabs';
import {GalleryModule, GalleryItem, ImageItem} from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessagesComponent } from "../member-messages/member-messages.component";
import { MessageService } from '../../_services/message.service';
import { AccountService } from '../../_services/authentication.service';
import { FavoritesService } from '../../_services/favorites.service';
@Component({
  selector: 'app-member-detail',
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessagesComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit, OnDestroy{
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent; // member tabs available while view is created
  private messageService = inject(MessageService);
  private accountService = inject(AccountService);
  private favoritesService = inject(FavoritesService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;
  hasLiked = computed(() => this.favoritesService.favoritesIds().includes(this.member.id));

  ngOnInit() {
    this.route.data.subscribe(
      {
        next: data => {this.member = data['member'];
          this.member && this.member.photos.forEach(photo => {
            this.images.push(
              new ImageItem({src: photo?.url, thumb: photo?.url}));
            });
          }
      }
    );

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab']);
      }
    });
  }

  selectTab(heading: string)
  {
    if(this.memberTabs)
    {
      const messageTab = this.memberTabs.tabs.find(x => x.heading === heading);
      if(messageTab)
        messageTab.active = true;
    }
  }
  toggleLike()
  {
    this.favoritesService.toggleLike(this.member.id).subscribe(
    {
      next: () => {
        if(this.hasLiked())
        {
          // already liked so we need to remove the like
          this.favoritesService.favoritesIds.update(ids => ids.filter(x => x != this.member.id));
        }
        else
        {
          // add the like
          this.favoritesService.favoritesIds.update(ids => [...ids, this.member.id]);
        }
      }
    });
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {tab: this.activeTab.heading},
      queryParamsHandling: 'merge'
    }); // pupulate the url with the tab heading
    if(this.activeTab.heading === 'Messages' && this.member)
    {
      const user = this.accountService.currentUser();
      if(user === null) return;
      this.messageService.createHubConnection(user, this.member.username);
    }
    else
    {
      this.messageService.stopHubConnection();
    }
  }

  ngOnDestroy() {
    this.messageService.stopHubConnection();
  }
}
