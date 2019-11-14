import { Component } from '@angular/core';
import { Global } from './model-data/global';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  isLogin = Global.isLogin;

  title = 'NgClientApp';
}
