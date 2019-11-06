import {
  Component,
  ViewChild,
  AfterViewInit
} from '@angular/core';

import * as DecoupledEditor from '@ckeditor/ckeditor5-build-decoupled-document';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-quillEditor-component',
  templateUrl: './quillEditor.component.html'
})
export class QuillEditorComponent implements AfterViewInit {
  @ViewChild('demoForm', { static: true }) public demoForm?: NgForm;

  public Editor = DecoupledEditor;
  public model = {
    name: 'John',
    surname: 'Doe',
    description: '<p>A <b>really</b> nice fellow.</p>'
  };

  public formDataPreview?: string;

  public get description() {
    return this.demoForm!.controls.description;
  }

  public ngAfterViewInit() {
    this.demoForm!.control.valueChanges.subscribe(values => {
      this.formDataPreview = JSON.stringify(values);
    });
  }

  public onSubmit() {
    console.log('Form submit, model', this.model);
  }

  public reset() {
    this.demoForm!.reset();
  }
}
