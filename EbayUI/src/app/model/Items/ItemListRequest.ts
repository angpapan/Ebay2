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
    if(params['location'])this.locations = params['location'];
  }

  querify() : string{

    return $.param(this).replaceAll(`%5D`,'').replaceAll(`%5B`,'');

  }



}
