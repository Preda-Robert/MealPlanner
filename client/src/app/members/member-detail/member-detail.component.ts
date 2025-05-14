import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { GalleryModule } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  standalone: true,
  imports: [CommonModule, GalleryModule, TimeagoModule, MemberMessagesComponent]
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent;
  member: MemberDTO = {} as MemberDTO;
  images: any[] = [];
  activeTab?: TabDirective;

  constructor(
    private memberService: MemberService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const username = this.route.snapshot.paramMap.get('username');
    if (!username) return;

    this.memberService.getMember(username).subscribe({
      next: member => {
        this.member = member;
        this.loadGalleryImages();
      }
    });
  }

  loadGalleryImages() {
    // This is assuming you have photo entities in your member object
    // Adjust according to your actual data structure
    if (this.member.photoUrl) {
      this.images = [{ src: this.member.photoUrl, thumb: this.member.photoUrl }];
    }

    // If you have a collection of photos, you might do something like:
    // this.images = this.member.photos?.map(photo => {
    //   return { src: photo.url, thumb: photo.url };
    // }) || [];
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      const tab = this.memberTabs.tabs.find(tab => tab.heading === heading);
      if (tab) {
        tab.active = true;
      }
    }
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
  }

  toggleLike() {
    this.memberService.toggleLike(this.member.userName).subscribe({
      next: () => {
        this.loadMember();
      }
    });
  }

  hasLiked(): boolean {

    return false;
  }
}
