// home.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faPlateWheat,
  faCartShopping,
  faKitchenSet
} from '@fortawesome/free-solid-svg-icons';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

interface Feature {
  title: string;
  description: string;
  icon: any;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FontAwesomeModule, RouterLink, RouterLinkActive],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  faPlateWheat = faPlateWheat;
  faCartShopping = faCartShopping;
  faKitchenSet = faKitchenSet;

  features: Feature[] = [
    {
      title: 'Personalized Meal Plans',
      description: 'Tailored recipes that fit your dietary preferences and lifestyle.',
      icon: this.faPlateWheat
    },
    {
      title: 'Smart Grocery Lists',
      description: 'Automated shopping lists that save you time and reduce waste.',
      icon: this.faCartShopping
    },
    {
      title: 'Easy Cooking',
      description: 'Step-by-step recipes that make cooking a breeze.',
      icon: this.faKitchenSet
    }
  ];
}
