import {PaginationRequest} from "../PaginationRequest";

export class OutboxRequest extends PaginationRequest {
  "search"?: string | undefined;
}
