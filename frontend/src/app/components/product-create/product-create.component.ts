import { Component } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-product-create',
  templateUrl: './product-create.component.html'
})
export class ProductCreateComponent {
  product: Product = { name: '', price: 0, stockQty: 0 };
  selectedFile: File | null = null;

  constructor(private productService: ProductService) {}

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  saveProduct() {
    this.productService.createProduct(this.product).subscribe(res => {
      if (this.selectedFile && res.productId) {
        this.productService.uploadImage(res.productId, this.selectedFile).subscribe(() => {
          alert('Product and Image saved successfully!');
        });
      }
    });
  }
}
