export interface UrlDto {
  id: number;
  originalUrl: string;
  shortCode: string;
  dateCreated: string;
  createdBy?: string;
  clickCount: number;
}

export interface CreateUrlRequest {
  originalUrl: string;
}
