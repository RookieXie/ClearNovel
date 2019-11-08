import { Component, OnInit } from '@angular/core';
import { Hero } from '../model-data/hero';
import { HeroService } from '../services/hero.service';
@Component({
  selector: 'app-heros',
  templateUrl: './heros.component.html',
  styleUrls: ['./heros.component.css']
})
export class HerosComponent implements OnInit {
  constructor(private heroService: HeroService) { }
  heros: Hero[];
  selectedHero: Hero;
  getHeroes(): void {
    this.heroService.getHeroes()
      .subscribe(heroes => this.heros = heroes);
  }

  ngOnInit() {
    this.getHeroes();
  }
  onSelect(hero: Hero): void {
    this.selectedHero = hero;
  }

}
