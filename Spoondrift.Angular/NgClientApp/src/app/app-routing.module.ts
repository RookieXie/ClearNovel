import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HerosComponent } from './heros/heros.component';
import { MyEditorComponent } from './my-editor/my-editor.component'; 


const routes: Routes = [
  { path: 'heros', component: HerosComponent },
  { path: 'my-editor', component: MyEditorComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
