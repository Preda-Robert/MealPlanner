// learn-more.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faCheckCircle,
  faClock,
  faUserFriends,
  faDatabase,
  faPuzzlePiece,
  faChartLine
} from '@fortawesome/free-solid-svg-icons';

interface Feature {
  title: string;
  description: string;
  icon: any;
}

@Component({
  selector: 'app-learn-more',
  standalone: true,
  imports: [CommonModule, FontAwesomeModule],
  templateUrl: './learn-more.component.html',
  styleUrl: './learn-more.component.css'
})
export class LearnMoreComponent {
  faCheckCircle = faCheckCircle;
  faClock = faClock;
  faUserFriends = faUserFriends;
  faDatabase = faDatabase;
  faPuzzlePiece = faPuzzlePiece;
  faChartLine = faChartLine;

  coreFeatures: Feature[] = [
    {
      title: 'Personalized Recommendations',
      description: 'AI-powered meal suggestions tailored to your dietary preferences, health goals, and taste.',
      icon: this.faPuzzlePiece
    },
    {
      title: 'Time-Saving Recipes',
      description: 'Quick and easy recipes that fit your busy lifestyle, with step-by-step guidance.',
      icon: this.faClock
    },
    {
      title: 'Smart Grocery Lists',
      description: 'Automated shopping lists that minimize waste and optimize your grocery shopping.',
      icon: this.faCheckCircle
    }
  ];

  performanceFeatures: Feature[] = [
    {
      title: 'Nutritional Tracking',
      description: 'Detailed nutritional insights to help you meet your health and fitness goals.',
      icon: this.faChartLine
    },
    {
      title: 'Community Recipes',
      description: 'Access thousands of user-submitted recipes and share your own culinary creations.',
      icon: this.faUserFriends
    },
    {
      title: 'Extensive Recipe Database',
      description: 'Over 10,000 recipes covering various cuisines, diets, and cooking styles.',
      icon: this.faDatabase
    }
  ];
}
