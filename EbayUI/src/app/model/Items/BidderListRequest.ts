import {PaginationRequest} from "../PaginationRequest";

export class BidderListRequest extends PaginationRequest {
  "orderBy"?: string | undefined;
  "search"?: string | undefined;
}
