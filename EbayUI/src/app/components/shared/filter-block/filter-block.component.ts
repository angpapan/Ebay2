import {Component, Input, OnInit} from '@angular/core';
import {CategoryService} from "../../../Services/category.service";
import {Category} from "../../../model/Category";
import {ItemListRequest} from "../../../model/Items/ItemListRequest";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-filter-block',
  templateUrl: './filter-block.component.html',
  styleUrls: ['./filter-block.component.css']
})
export class FilterBlockComponent implements OnInit {
  @Input() selectedCategories : number[] | undefined;
  @Input() pageInfo : ItemListRequest;
  Categories : Category[];
  minPrice : number;
  maxPrice : number;
  result : categoryCheck[];

  constructor(private _categoryService : CategoryService, private router : Router) { }

  ngOnInit(): void {
    this._categoryService.getCategories().subscribe({next:response=>{
      this.Categories = response;
      this.result = this.Categories.map((v)=> new categoryCheck(v,this.selectedCategories) );
      //console.log(this.result);
      }})
  }

  search(){

    this.pageInfo.categories = this.result.filter(v=>v.isChecked).map(v=>v.category.categoryId);
    this.router.navigate( [`search`], {queryParams:new helper(this.pageInfo)}  ).then();

  }

}

class categoryCheck{

  public category : Category;
  public isChecked : boolean | undefined = false;

  constructor(cat : Category, cur? : number[]) {
    this.category = cat;
    this.isChecked = cur && cur.some(i=>i == this.category.categoryId);
  }

}

class helper{
  "orderBy"?: string | undefined;
  "minPrice"? : number | undefined;
  "maxPrice"? : number | undefined;
  "categories"? : number[] | undefined;
  "locations"? : string[] | undefined;

  constructor(original : ItemListRequest) {
    if(original.orderBy) this.orderBy = original.orderBy;
    if(original.minPrice) this.minPrice = original.minPrice;
    if(original.maxPrice) this.maxPrice = original.maxPrice;
    if(original.categories) this.categories = original.categories;
    if(original.locations) this.locations = original.locations;
  }
  queryfy(){
    return $.param(this).replaceAll(`%5D`,'').replaceAll(`%5B`,'');
  }
}

