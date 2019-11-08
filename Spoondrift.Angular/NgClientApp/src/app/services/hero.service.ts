import { Injectable } from '@angular/core';
import { Hero } from '../model-data/hero';
import { Heros } from '../model-data/hero.mock';
import { Observable, of } from 'rxjs';

import { MessageService } from '../services/message.service';
@Injectable({
  providedIn: 'root'
})
export class HeroService {

  constructor(private messageService: MessageService) { }
  getHeroes(): Observable<Hero[]> {
    this.messageService.add('HeroService: fetched heroes');
    return of(Heros);
  }
}
