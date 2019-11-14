import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HerosComponent } from './heros/heros.component';
import { FormsModule } from '@angular/forms';
import { MyEditorComponent } from './my-editor/my-editor.component'; // <-- NgModel lives here
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { HeroDetailComponent } from './hero-detail/hero-detail.component';
import { MessagesComponent } from './messages/messages.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { LoginComponent } from './login/login.component';
import { AuthFilter} from './auth-filter/auth-filter';

@NgModule({
  declarations: [
    AppComponent,
    HerosComponent,
    MyEditorComponent,
    HeroDetailComponent,
    MessagesComponent,
    NavMenuComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    CKEditorModule,
    NgbModule
  ],
  providers: [AuthFilter],
  bootstrap: [AppComponent]
})
export class AppModule { }
