import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HerosComponent } from './heros/heros.component';
import { MyEditorComponent } from './my-editor/my-editor.component';
import { LoginComponent } from './login/login.component';
import { AuthFilter } from './auth-filter/auth-filter';


const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'heros', component: HerosComponent, canActivate: [AuthFilter] },
  { path: 'my-editor', component: MyEditorComponent, canActivate: [AuthFilter] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
