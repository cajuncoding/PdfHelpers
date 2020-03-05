# PdfHelpers.Resize
Lightweight Helper Library for scaling and resizing Pdf Documents and Pages of existing Pdf documents.  It leverages iTextSharp for core work but offeres a easy wrapper for common resizing needs.

It's as easy as . . .

### Sample for Resizing Pdf Content:
```
byte[] pdfBytes = File.ReadAllBytes("...path to pdf...");
var targetSizeInfo = new PdfResizeInfo(PageSize.POSTCARD, PdfMarginSize.None);
byte[] scaledPdfBytes = PdfResizeHelper.ResizePdfPageSize(pdfBytes, targetSizeInfo, PdfScalingOptions.Default)
```
  
  
#### NOTES:
**Loss of Interactive Elements from Pdfs:**  
iTextSharp does not allow manipulation of content when using the PdfCopy or PdfSmartCopy classes for Pdf merging, therefore the PdfWriter 
must be used to make changes to content such as scaling it -- as opposed to just adding overlay/underlay content.  This has the side effect of causing
loss of interactive functions of pages that are resized such as Annotations, Form Fields, etc.  However for many use cases this is perfectly valid implilcation
when resizing content as Annotations would lose their positions, and form fields could become unusable, etc.  So for Read-only content the concept of scaling/re-sizing
will work with the approach used here.

**Additional information:**
Great overview of merging and manipulating Pdfs with iTextSharp (e.g. iText) can be found in the free online book chapter -- written by the authoer of iText -- here:  
https://livebook.manning.com/book/itext-in-action-second-edition/chapter-6/

```
/*
MIT License

Copyright (c) 2018

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
```
