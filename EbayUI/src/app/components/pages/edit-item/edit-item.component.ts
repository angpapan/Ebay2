import {Component, OnInit, ViewChild} from '@angular/core';
import {DataTableDirective} from "angular-datatables";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Category} from "../../../model/Category";
import {firstValueFrom, Subject} from "rxjs";
import {ItemService} from "../../../Services/item.service";
import {CurrencyPipe} from "@angular/common";
import {CategoryService} from "../../../Services/category.service";
import {SwalService} from "../../../Services/swal.service";
import Swal from "sweetalert2";
import {ActivatedRoute, Router} from "@angular/router";
import {Base64WithIdImage} from "../../../model/Images/Base64WithIdImage";
import {DomSanitizer} from "@angular/platform-browser";

@Component({
  selector: 'app-edit-item',
  templateUrl: './edit-item.component.html',
  styleUrls: ['./edit-item.component.css']
})
export class EditItemComponent implements OnInit {

  @ViewChild(DataTableDirective, {static: false})
  dtElement: DataTableDirective;

  newItemForm: FormGroup;
  _images: File[] = [];
  _imagesPreview: string[] = [];
  _existingImages: Base64WithIdImage[] =[];
  _categories: Category[] = [];
  _selectedCategories: Category[] = [];
  dtOptions: DataTables.Settings;
  dtTrigger: Subject<any> = new Subject<any>();
  gettingCoordinates: boolean = false;
  itemId: number;


  constructor(private itemService: ItemService, private currencyPipe: CurrencyPipe,
              private swalService: SwalService, private route: ActivatedRoute,
              private sanitizer: DomSanitizer, private router: Router) { }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

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
      'buyPrice': new FormControl(''),
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
      'ends': new FormControl("", {
          validators: [
            Validators.required,
          ],
        }
      ),
      'latitude': new FormControl(''),
      'longitude': new FormControl(''),
      'categoriesId': new FormControl('', {
        validators: [
          Validators.required
        ]
      }),
      'imageFiles': new FormControl(''),
    });

    this.dtOptions = {
      columns: [{
        title: 'Name',
        data: 'name'
      }, {
        title: 'Add',
      }]
    }


    this.route.params.subscribe(params => {
      this.itemId = params['id'];
      this.itemService.getEditInfo(this.itemId).subscribe(info => {
        console.log(info);
        this.name?.setValue(info.name ?? '');
        this.description?.setValue(info.description ?? '');
        this.buyPrice?.setValue(info.buyPrice ? this.convertToCurrency(info.buyPrice) : '');
        this.firstBid?.setValue(info.firstBid ? this.convertToCurrency(info.firstBid) : '');
        this.location?.setValue(info.location ?? '');
        this.country?.setValue(info.country ?? '');
        this.ends?.setValue(info.ends ?? '');
        this.latitude?.setValue(info.latitude ?? '');
        this.longitude?.setValue(info.longitude ?? '');
        this.categoriesId?.setValue(info.addedCategories.map(c => c.categoryId));

        if(info.currentImages){
          this._existingImages = info.currentImages.map(i => {
            const byteArray = new Uint8Array(atob(i.data as string).split('').map(char => char.charCodeAt(0)));
            let bl: Blob = new Blob([byteArray]);
            const source = this.sanitizer.bypassSecurityTrustUrl(window.URL.createObjectURL(bl));

            let image: Base64WithIdImage = new Base64WithIdImage();
            image.imageId = i.imageId;
            image.data = source;

            return image;
          })
        }

        this._selectedCategories = info.addedCategories ?? [];
        this._categories = info.restCategories ?? [];
        this.dtTrigger.next('');
      });
    })

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
  get categoriesId() { return this.newItemForm.get('categoriesId'); }
  get images() { return this.newItemForm.get('imageFiles'); }

  addCategory(cat: Category){
    console.log(cat);
    this._categories = this._categories.filter(c => c !== cat);
    this._selectedCategories = [...this._selectedCategories, cat]

    console.log(this._categories)
    console.log(this._selectedCategories)
    if(this._categories.length > 0)
      this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
        dtInstance.destroy();
        this.dtTrigger.next(this.dtOptions);
      })

    this.newItemForm.patchValue({
      categoriesId: this._selectedCategories.map(c => c.categoryId)
    });

    this.categoriesId?.updateValueAndValidity();
  }

  removeCategory(cat: Category){
    console.log(this.categoriesId);
    this._selectedCategories = this._selectedCategories.filter(c => c !== cat);
    this._categories = [...this._categories, cat]

    if(this._categories.length > 0)
      this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
        dtInstance.destroy();
        this.dtTrigger.next(this.dtOptions);
      })

    this.newItemForm.patchValue({
      categoriesId: this._selectedCategories.map(c => c.categoryId)
    });

    this.categoriesId?.updateValueAndValidity();
  }

  transformAmount(element: any) {
    let value = element.target.value;
    const form_id = element.target.id;

    value = this.convertToCurrency(value);

    this.newItemForm.patchValue({ [form_id]: value });
  }

  convertToCurrency(value: string | number): string {
    value = String(value);
    value = value.replace(/[^\d.-]/g, '');
    try {
      return this.currencyPipe.transform(value, 'EUR')!;
    }
    catch {
      return '';
    }
  }


  onFileChange(event: any) {
    if (event.target.files && event.target.files[0]) {
      console.log(event.target.files);
      const filesAmount = event.target.files.length;
      for (let i = 0; i < filesAmount; i++) {

        console.log('FILE OUTSIDE:', event.target.files[i]);
        this._images.push(event.target.files[i]);

        this.newItemForm.patchValue({
          imageFiles: this._images
        });
        this.images?.updateValueAndValidity();
      }

      this.loadImagesPreview();
    }
  }

  loadImagesPreview() {
    this._imagesPreview = [];
    let that = this;
    for(let i=0; i < this._images.length; i++){
      const reader = new FileReader();

      reader.onload = (event: any) => {
        //console.log(event.target.result);
        that._imagesPreview.push(event.target.result);
      }

      reader.readAsDataURL(this._images[i]);

    }
  }

  removeImage(index: number) {
    this._images.splice(index , 1);
    this.newItemForm.patchValue({
      imageFiles: this._images
    });
    this.images?.updateValueAndValidity();

    this.loadImagesPreview();
  }

  deleteImageFromItem(image_id: number){
    Swal.fire({
      ...this.swalService.BootstrapOptions,
      icon:'warning',
      html:'Are you sure that you want to delete this image?'
    }).then(result => {
      if(result.isConfirmed){
        this.itemService.deleteImage(this.itemId, image_id).subscribe({
          next: response => {
            Swal.fire({
              ...this.swalService.BootstrapConfirmOnlyOptions,
              icon:'success',
              html:'Image deleted successfully'
            })
            this._existingImages = this._existingImages.filter(i => i.imageId !== image_id)
          }
        })

      }
    })
  }

  getCurrentLocation() {
    this.gettingCoordinates = true;
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          let lat = position.coords.latitude;
          let long = position.coords.longitude;

          this.newItemForm.patchValue({
            latitude: lat
          });
          this.newItemForm.patchValue({
            longitude: long
          });
          this.latitude?.updateValueAndValidity();
          this.longitude?.updateValueAndValidity();
          this.gettingCoordinates = false;
        },
        (err) => {
          if(err.code !== err.PERMISSION_DENIED)
            Swal.fire({
              ...this.swalService.BootstrapConfirmOnlyOptions,
              icon: 'error',
              html: "Sorry, something went wrong. Please try selecting your position manually."
            })

          this.gettingCoordinates = false;
        },
        {
          enableHighAccuracy: true
        }
      );
    } else {
      Swal.fire({
        ...this.swalService.BootstrapConfirmOnlyOptions,
        icon: 'error',
        html: "Geolocation is not supported. Please try selecting your position manually."
      })
    }
  }

  submit() {
    if(this.newItemForm?.invalid){
      Swal.fire({
        ...this.swalService.BootstrapConfirmOnlyOptions,
        icon: 'error',
        html: 'The form is invalid. Please check the fields again.'
      })
      return;
    }

    this.firstBid?.setValue(this.firstBid.value.replace(/[^\d.-]/g, ''));
    this.buyPrice?.setValue(this.buyPrice.value.replace(/[^\d.-]/g, ''));

    let formData: any = new FormData();
    formData.append("name", this.name?.value);
    formData.append("description", this.description?.value);
    formData.append("location", this.location?.value);
    formData.append("buyPrice", this.buyPrice?.value);
    formData.append("firstBid", this.firstBid?.value);
    formData.append("country", this.country?.value);
    formData.append("ends", this.ends?.value);
    formData.append("latitude", this.latitude?.value);
    formData.append("longitude", this.longitude?.value);
    for(let cat of this.categoriesId?.value){
      formData.append("categoriesId", cat);
    }
    Array.from(this.images?.value).map((file: any) => {
      return formData.append('imageFiles', file, file.name);
    });

    for(let [key, value] of formData.entries()){
      console.log(key, value);
    }
    // return;
    Swal.fire({
      ...this.swalService.BootstrapOptions,
      icon: 'question',
      html: 'Are you sure you want to modify this auction?',
      preConfirm: () => {
        return firstValueFrom(this.itemService.editAuction(this.itemId, formData));
      }
    }).then(response => {
      console.log(response);
      if(response.isConfirmed) {
        if (response!.value) {
          Swal.fire({
            ...this.swalService.BootstrapConfirmOnlyOptions,
            icon: 'success',
            html: 'Auction modified successfully.'
          }).then(r => {
            this.router.navigate(["/items/seller-list"]);
          });
        }
      }
    })
  }

}
