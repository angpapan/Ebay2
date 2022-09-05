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
  @Input() pageInfo : ItemListRequest;
  Categories : Category[];
  result : categoryCheck[];
  filterdPage : ItemListRequest;

  constructor(private _categoryService : CategoryService, private router : Router) { }

  ngOnInit(): void {

    this.filterdPage = new ItemListRequest();
    this.filterdPage.maxPrice = this.pageInfo.maxPrice;
    this.filterdPage.minPrice = this.pageInfo.minPrice;
    this._categoryService.getCategories().subscribe({next:response=>{
      this.Categories = response;
      this.result = this.Categories.map((v)=> new categoryCheck(v,this.pageInfo.categories) );
      }})
  }

  search(){

    this.filterdPage.orderBy = this.pageInfo.orderBy;
    this.filterdPage.locations = this.pageInfo.locations;
    this.filterdPage.categories = this.result.filter(v=>v.isChecked).map(v=>v.category.categoryId);
    this.router.navigate( [`search`], {queryParams:this.filterdPage.reduceParameters()}  ).then();

  }

}

class categoryCheck{

  public category : Category;
  public isChecked : boolean | undefined = false;

  constructor(cat : Category, cur? : number[] ) {
    this.category = cat;
    if(Array.isArray(cur))
      this.isChecked = cur && cur.some(i=>i == this.category.categoryId);
    else
      this.isChecked = this.category.categoryId == cur;
  }

}


