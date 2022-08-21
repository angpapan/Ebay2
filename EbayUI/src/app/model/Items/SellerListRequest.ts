import {PaginationRequest} from "../PaginationRequest";

export class SellerListRequest extends PaginationRequest {
  "orderBy"?: string | undefined;
  "search"?: string | undefined;
}
