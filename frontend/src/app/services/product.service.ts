import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Product } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductService {
  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Product[]>(environment.productApiUrl);
  }

  create(product: Product) {
    return this.http.post<Product>(environment.productApiUrl, product);
  }

  // The 'Must Have' Image Upload Logic
  uploadImage(id: number, file: File) {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<{ imageUrl: string }>(
      `${environment.productApiUrl}/${id}/image`, 
      formData
    );
  }
      }
