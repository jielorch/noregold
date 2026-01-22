import { inject, Injectable } from "@angular/core";
import { ApiService } from "../services/api.service";
import { Observable } from "rxjs";
import { Inventory } from "../../features/pages/inventory/inventory";

@Injectable({
  providedIn: 'root'
})

export class FileUploadRepository{
    
    private apiService = inject(ApiService);

    private controller = 'inventory';
    private getInventory = 'get';
    private upload = 'upload';

    get():Observable<Inventory[]>{
        return this.apiService.get(this.controller, this.getInventory);
    }

    uploadFile(form: FormData):Observable<boolean>{
        return this.apiService.post(this.controller, this.upload, form);
    }
}