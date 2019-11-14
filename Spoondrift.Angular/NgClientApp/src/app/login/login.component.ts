import { Component, OnInit, Input } from '@angular/core';
import { Global } from '../model-data/global';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  @Input() email = '';
  @Input() password = '';
  constructor(private router: Router) { }

  ngOnInit() {
  }
  login() {
    this.router.navigate(['/heros']);
  }
}
