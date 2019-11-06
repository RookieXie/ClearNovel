import { Component, OnInit } from '@angular/core';
import { Hero } from './hero';
import { Heros } from './hero.mock';
@Component({
  selector: 'app-heros',
  templateUrl: './heros.component.html',
  styleUrls: ['./heros.component.css']
})
export class HerosComponent implements OnInit {
  heros = Heros;
  constructor() { }

  ngOnInit() {
  }

}
