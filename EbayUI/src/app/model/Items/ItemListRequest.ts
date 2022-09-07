import { PaginationRequest } from "../PaginationRequest";

export class ItemListRequest extends PaginationRequest{
  "orderBy"?: string | undefined;
  "minPrice"? : number | undefined;
  "maxPrice"? : number | undefined;
  "categories"? : number[] | undefined;
  "locations"? : string[] | undefined;

  serialize(params: any) : void{
    if(params['orderBy']) this.orderBy = params['orderBy'];
    if(params['minPrice']) this.minPrice = params['minPrice'];
    if(params['maxPrice']) this.maxPrice = params['maxPrice'];
    if(params['categories'])this.categories = params['categories'];
    if(params['locations'])this.locations = params['locations'];
  }

  reduceParameters(): helper{
    return new helper(this);
  }

  querify() : string{

    return $.param(this).replaceAll(`%5D`,'').replaceAll(`%5B`,'');

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
    console.log(this);
  }

}
