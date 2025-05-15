import { Component, Input } from '@angular/core';
import { Recipe } from '../_models/recipe';
import { CommonModule } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'orderBy' })
export class OrderByPipe implements PipeTransform {
  transform(array: any[], field: string): any[] {
    if (!array || !field) return array;
    return array.sort((a, b) => a[field] - b[field]);
  }
}

@Component({
  selector: 'app-view-recipe',
  standalone: true,
  imports: [CommonModule, OrderByPipe],
  templateUrl: './view-recipe-details.component.html',
  styleUrls: ['./view-recipe-details.component.css']
})
export class ViewRecipeComponent {
  @Input() recipe: Recipe | null = null;
}
