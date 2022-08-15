import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-try-upload',
  templateUrl: './try-upload.component.html',
  styleUrls: ['./try-upload.component.css']
})
export class TryUploadComponent implements OnInit {

  images: string[] = [];

  /*------------------------------------------
  --------------------------------------------
  Declare Form
  --------------------------------------------
  --------------------------------------------*/
  myForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.minLength(3)]),
    file: new FormControl('', [Validators.required]),
    fileSource: new FormControl([''], [Validators.required])
  });

  ngOnInit() { }

  constructor(private http: HttpClient) { }

  get f() {
    return this.myForm.controls;
  }

  onFileChange(event: any) {
    if (event.target.files && event.target.files[0]) {
      var filesAmount = event.target.files.length;
      for (let i = 0; i < filesAmount; i++) {
        var reader = new FileReader();

        reader.onload = (event: any) => {
          console.log(event.target.result);
          this.images.push(event.target.result);

          this.myForm.patchValue({
            fileSource: this.images
          });
        }

        reader.readAsDataURL(event.target.files[i]);
      }
    }
  }


  submit() {
    console.log(this.myForm.value);
    this.http.post('https://localhost:7088/item/upload-images', this.myForm.value)
      .subscribe(res => {
        console.log(res);
        alert('Uploaded Successfully.');
      })
  }
}
