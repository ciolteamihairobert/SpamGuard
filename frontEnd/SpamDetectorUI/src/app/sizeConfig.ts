import { Injectable } from "@angular/core";
import { MatDialogConfig } from "@angular/material/dialog";

@Injectable({
    providedIn: 'root',
})

export class ModalSize{
    configLargeSU: MatDialogConfig<any> = {
        height: '40%',
        width: '40%'
      };
    configSmallSU: MatDialogConfig<any> = {
        height: '85%',
        width: '60%'
      };
    configLargeSI: MatDialogConfig<any> = {
        height: '30%',
        width: '30%'
      }
    configSmallSI: MatDialogConfig<any> = {
        height: '65%',
        width: '50%'
      }
}