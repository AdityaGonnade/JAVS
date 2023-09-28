import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private baseUrl : string = "https://localhost:7031/api/User/";
  constructor(private http: HttpClient) { }

  sendEmailToUser(id : string){
    return this.http.post<any>(`${this.baseUrl}/email-notification/${id}`,{});
  }

  // sendEmailToVendor(email : string){
  //   return this.http.post<any>(`${this.baseUrl}/email-vendor-notification/${email}`,{});
  // }
}
