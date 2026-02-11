import { Component } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-create',
  templateUrl: './product-create.component.html'
})
export class ProductCreateComponent {
  product: Product = { name: '', price: 0, stockQty: 0 };
  selectedFile: File | null = null;

  constructor(private productService: ProductService, private router: Router) {}

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  onSubmit() {
    // 1. Create Product
    this.productService.create(this.product).subscribe(createdProduct => {
      // 2. Upload Image if selected
      if (this.selectedFile && createdProduct.productId) {
        this.productService.uploadImage(createdProduct.productId, this.selectedFile)
          .subscribe(() => this.router.navigate(['/products']));
      } else {
        this.router.navigate(['/products']);
      }
    });
  }
}
