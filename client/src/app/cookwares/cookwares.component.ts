import { NgClass } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { CookwareService } from '../_services/cookware.service';

@Component({
  selector: 'app-cookwares',
  imports: [FormsModule, PaginationModule],
  templateUrl: './cookwares.component.html',
  styleUrl: './cookwares.component.css'
})
export class CookwaresComponent {
  searchQuery: string = '';
  cookwareService = inject(CookwareService);


  onSearch() {
    this.getPaginatedCookwares();
  }

  ngOnInit(): void {
    this.cookwareService.getCookwares();
  }

  onCookwarePageChanged(event: any) {
    if (this.cookwareService.cookwareParams().pageNumber !== event.page) {
      this.cookwareService.cookwareParams().pageNumber = event.page;
      this.loadCookwares();
    }
  }

  loadCookwares() {
    this.getPaginatedCookwares();
  }

  getPaginatedCookwares() {
    console.log("getting paginated cookwares");
    this.cookwareService.getCookwares(this.searchQuery);
  }
}
