import { Component, OnInit } from '@angular/core';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';

@Component({
  selector: 'app-my-editor',
  templateUrl: './my-editor.component.html',
  styleUrls: ['./my-editor.component.css']
})
export class MyEditorComponent implements OnInit {
  public Editor = ClassicEditor;
  constructor() { }

  ngOnInit() {
  }

}
