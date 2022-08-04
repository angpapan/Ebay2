export class ItemBoxItem {
  "id": number;
  "title": string = "";
  "description": string = "";
  "image"?: string | undefined;

  constructor(id: number, title: string, description: string) {
    this.id = id;
    this.title = title;
    this.description = description;
  }
}
