import {Category} from "../Category";
import {Base64WithIdImage} from "../Images/Base64WithIdImage";

export class EditItemInfoResponse {
  "name": string;
  "buyPrice"?: number;
  "firstBid": number;
  "location"?: string;
  "country"?: string;
  "description": string;
  "ends": Date;
  "latitude"?: number;
  "longitude"?: number;
  "addedCategories": Category[];
  "restCategories"?: Category[];
  "currentImages"?: Base64WithIdImage[]
}
