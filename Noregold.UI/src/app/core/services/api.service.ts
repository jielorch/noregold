import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ApiService{

    private baseUrl = environment.apiBaseUrl
     private headers = new HttpHeaders().set('content-type', 'application/json; charset=utf-8');
    
    constructor(private httpClient: HttpClient) { }

    post(controller: string, endpoint: string, param: any, withHeaders:boolean = false):Observable<any>{
        const options = withHeaders ? { headers: this.headers } : {};
        return this.httpClient.post<any>(`${this.baseUrl}/${controller}/${endpoint}`, param, options);
    }

    put(controller: string, endpoint: string, param: any): Observable<any>{
        return this.httpClient.put<any>(`${this.baseUrl}/${controller}/${endpoint}`, param);
    }

    get(controller: string, endpoint: string, params?: any): Observable<any>{
        let url = `${this.baseUrl}/${controller}/${endpoint}`;
        let httpParams = new HttpParams();
        if(params){
            if(typeof params === 'object' && params !== null){
                 Object.keys(params).forEach(key => {
                    httpParams = httpParams.set(key, params[key]);
                 });
            }else{
                url += `/${params}`;
            }
        }
        return this.httpClient.get<any>(url, { params: httpParams });
    }
}