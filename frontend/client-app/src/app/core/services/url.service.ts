import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {CreateUrlRequest, UrlDto} from '../../shared/models/url.model';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class UrlService {
  private apiUrl = '/api/url';

  constructor(private http: HttpClient) {}

  getHistory(): Observable<UrlDto[]> {
    return this.http.get<UrlDto[]>(
      `${this.apiUrl}/history`,
    )
  }

  shorten(originalUrl: string): Observable<UrlDto> {
    const body: CreateUrlRequest = {originalUrl: originalUrl};

    return this.http.post<UrlDto>(
      `${this.apiUrl}/shorten`,
      body,
      { withCredentials: true }
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`,
      { withCredentials: true }
    );
  }

  getUrlById(id: number): Observable<UrlDto> {
    return this.http.get<UrlDto>(
      `${this.apiUrl}/${id}`,
      { withCredentials: true }
    );
  }
}
