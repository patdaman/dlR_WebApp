module BillingSuiteApp.Controller {
    export function DateToUSString(indate: Date): string {
        return (indate.getMonth() + 1).toString() + "/" +
            indate.getDate().toString() + "/" +
            indate.getFullYear().toString();
    }
}