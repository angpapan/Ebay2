import {PaginationRequest} from "./PaginationRequest";

export class UserListRequest extends PaginationRequest {
  "orderBy"?: string | undefined;
  "search"?: string | undefined;
}
