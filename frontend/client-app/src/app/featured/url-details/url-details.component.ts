import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { UrlService } from '../../core/services/url.service';
import { UrlDto } from '../../shared/models/url.model';

@Component({
  selector: 'app-url-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './url-details.component.html',
  styleUrl: './url-details.component.scss'
})
export class UrlDetailsComponent implements OnInit {
  url: UrlDto | null = null;
  isLoading = true;
  error = '';

  constructor(
    private route: ActivatedRoute,
    private urlService: UrlService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (id) {
      this.urlService.getUrlById(id).subscribe({
        next: (data) => {
          console.log('Дані отримано:', data);
          this.url = data;
          this.isLoading = false;

          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error(err);
          this.error = 'Запис не знайдено';
          this.isLoading = false;

          this.cdr.detectChanges();
        }
      });
    }
  }

  goBack() {
    this.router.navigate(['/urls']);
  }
}
