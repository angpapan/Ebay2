import { CurrencyPipe } from '@angular/common';
import { HttpClient, HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-new-item',
  templateUrl: './new-item.component.html',
  styleUrls: ['./new-item.component.css']
})
export class NewItemComponent implements OnInit {
  newItemForm: FormGroup;
  _images: string[] = [];

  constructor(private http: HttpClient, private currencyPipe: CurrencyPipe) { }

  ngOnInit(): void {
    this.newItemForm = new FormGroup({
      'name': new FormControl("", {
        validators: [
          Validators.required,
        ],
      }
      ),
      'description': new FormControl("", {
        validators: [
          Validators.required,
        ],
      }
      ),
      'buyPrice': new FormControl('', {
        validators: [
          Validators.required,
        ],
      }
      ),
      'firstBid': new FormControl("", {
        validators: [
          Validators.required,
        ],
      }
      ),
      'location': new FormControl("", {
        validators: [
          Validators.required,
        ],
      }
      ),
      'country': new FormControl("", {
        validators: [
          Validators.required,
        ],
      }
      ),
      'started': new FormControl(null),
      'ends': new FormControl("", {
        validators: [
          Validators.required,
        ],
      }
      ),
      'latitude': new FormControl(null),
      'longitude': new FormControl(null),
      'categories': new FormControl(null),
      'images': new FormControl(null),
    });
  }


  get name() { return this.newItemForm.get('name'); }
  get description() { return this.newItemForm.get('description'); }
  get buyPrice() { return this.newItemForm.get('buyPrice'); }
  get firstBid() { return this.newItemForm.get('firstBid'); }
  get location() { return this.newItemForm.get('location'); }
  get country() { return this.newItemForm.get('country'); }
  get started() { return this.newItemForm.get('started'); }
  get ends() { return this.newItemForm.get('ends'); }
  get latitude() { return this.newItemForm.get('latitude'); }
  get longitude() { return this.newItemForm.get('longitude'); }
  get categories() { return this.newItemForm.get('categories'); }


  //sendRequest = () => {
  //  const formData = new FormData();
  //  for (const key of Object.keys(this.newItemForm.value)) {
  //    const value = this.newItemForm.value[key];
  //    if(value !== null)
  //      formData.append(key, value);
  //  }

  //  this.http.post('https://localhost:7088/item/upload-images', formData, { reportProgress: true, observe: 'events' })
  //    .subscribe(
  //      {
  //        next: (event) => {
  //          if (event.type === HttpEventType.UploadProgress)
  //            this.progress = Math.round(100 * event.loaded / event.total!);
  //          else if (event.type === HttpEventType.Response) {
  //            this.message = 'Upload success.';
  //            //this.onUploadFinished.emit(event.body);
  //          }
  //        },
  //        error: (err: HttpErrorResponse) => console.log(err)
  //      });
  //}

  //uploadFile = (files: any) => {
  //  if (files.length === 0) {
  //    return;
  //  }
  //  let filesToUpload: File[] = files;

  //  console.log(filesToUpload);
  //  const formData = new FormData();

  //  Array.from(filesToUpload).map((file, index) => {
  //    console.log(file, file.name, index);
  //    return formData.append('file' + index, file, file.name);
  //  });

  //  //console.log(formData.entries);

  //  this.http.post('https://localhost:7088/item/upload-images', formData, { reportProgress: true, observe: 'events' })
  //    .subscribe(
  //      {
  //        next: (event) => {
  //          if (event.type === HttpEventType.UploadProgress)
  //            this.progress = Math.round(100 * event.loaded / event.total!);
  //          else if (event.type === HttpEventType.Response) {
  //            this.message = 'Upload success.';
  //            //this.onUploadFinished.emit(event.body);
  //          }
  //        },
  //        error: (err: HttpErrorResponse) => console.log(err)
  //      });
  //}

  transformAmount(element: any) {
    console.log(element);
    let value = element.target.value;
    value = value.replace(/[^\d.-]/g, '');
    console.log(value);
    const form_id = element.target.id;

    try {
      const price = this.currencyPipe.transform(value, 'EUR');

      this.newItemForm.patchValue({ [form_id]: price });
    }
    catch {
      this.newItemForm.patchValue({
        [form_id]: ''
      });
    }

    //element.target.value = price;
  }


  onFileChange(event: any) {
    if (event.target.files && event.target.files[0]) {
      var filesAmount = event.target.files.length;
      for (let i = 0; i < filesAmount; i++) {
        var reader = new FileReader();

        reader.onload = (event: any) => {
          //console.log(event.target.result);
          this._images.push(event.target.result);

          this.newItemForm.patchValue({
            images: this._images
          });
        }

        reader.readAsDataURL(event.target.files[i]);
      }
    }
  }

  submit() {
    console.log(this.newItemForm.value);
    this.http.post('https://localhost:7088/item/upload-images', this.newItemForm.value)
      .subscribe(res => {
        console.log(res);
        alert('Uploaded Successfully.');
      })
  }
}
