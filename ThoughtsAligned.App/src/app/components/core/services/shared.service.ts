import { EventEmitter, Injectable, Output } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  @Output() handleHeaderCss: EventEmitter<boolean> = new EventEmitter();
  constructor() { }
}
