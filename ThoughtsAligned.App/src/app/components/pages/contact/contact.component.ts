import { Component } from '@angular/core';
import { SharedService } from '../../core/services/shared.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent {
  constructor( private _sharedService: SharedService) { 
    this._sharedService.handleHeaderCss.emit(false);
  }
}
