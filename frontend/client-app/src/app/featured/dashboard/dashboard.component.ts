import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
import { UrlService } from '../../core/services/url.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  urls: any[] = [];
  isLoading = false;
  errorMessage = '';

  isLoggedIn = false;
  currentUserEmail: string | null = null;

  readonly redirectBaseUrl = 'http://localhost:5242/s/';

  urlInput = new FormControl('', [
    Validators.required,
    Validators.pattern(/^https?:\/\/.+/)
  ]);

  constructor(
    private urlService: UrlService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit() {
    this.checkAuth();
    this.loadHistory();
  }

  checkAuth() {
    this.isLoggedIn = localStorage.getItem('user') === 'true';
    this.currentUserEmail = localStorage.getItem('userEmail');
  }

  canDelete(url: any): boolean {
    if (!this.isLoggedIn) return false;

    return url.createdBy === this.currentUserEmail;
  }

  loadHistory() {
    this.isLoading = true;
    this.urlService.getHistory().subscribe({
      next: (data) => {
        this.urls = data.filter(u => u.shortCode && u.shortCode.length > 0);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Помилка завантаження:', err);
        this.isLoading = false;
      }
    });
  }

  onShorten() {
    if (this.urlInput.invalid) return;

    this.isLoading = true;
    const longUrl = this.urlInput.value!;

    this.urlService.shorten(longUrl).subscribe({
      next: (newUrl) => {
        this.urls.unshift(newUrl);
        this.urlInput.reset();
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.errorMessage = 'Помилка створення. Можливо, таке посилання вже є.';
        this.isLoading = false;
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  onDelete(id: number) {
    if (!confirm('Видалити це посилання?')) return;

    this.urlService.delete(id).subscribe({
      next: () => {
        this.urls = this.urls.filter(u => u.id !== id);
        this.cdr.detectChanges();
      },
      error: (err) => {
        alert('Не вдалося видалити. Можливо, у вас немає прав.');
      }
    });
  }

  logout() {
    localStorage.removeItem('user');
    localStorage.removeItem('userEmail');
    window.location.reload();
  }
}
