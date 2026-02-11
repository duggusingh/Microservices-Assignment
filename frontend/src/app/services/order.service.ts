import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order } from '../models/order.model';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private apiUrl = environment.orderApiUrl;
  constructor(private http: HttpClient) { }

  placeOrder(order: Order): Observable<any> {
    return this.http.post(this.apiUrl, order);
  }
}
