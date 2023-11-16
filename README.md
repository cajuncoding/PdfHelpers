# iTextSharp.PdfHelpers Project Overview:
A set of helpfer methods to greatly help with post-processing or pre-processing of PDF files using iTextSharp libraries. These elements provide very powerful capabilities when used in combination with a [dynamic templating appraoch to rendering PDF printable outputs (using Xsl-FO via MVC or Xslt)](https://github.com/cajuncoding/PdfTemplating.XslFO).

iTextSharp is a powerful Pdf processing library, but implementing correctly can take more effort/code than expected. In addition, there are sometimes multiple ways to accomplish some of these tasks, but not all appraoches are the most efficient.

So these helpers provide a simplified facade for doing common pre/post processing tasks, and help to dramatically decrease the amount of code and effort needed to meet your requirements for generating dynamic printable Pdf outputs.

#### Some use-cases:
 - Allow dynamic data-driven real-time rendered outputs to be combined with other Pdf content, or Images, uploaded by users (e.g. invoices, backup content, etc.)
 - Allow pre-defined Pdf content or Images to be pre-pended/appended to dynamically rendered Pdf content
 - Allow dynamic resizing (scale-up, scale-down, etc.) of imagery when added to existing, or dynamically rendered, Pdf content

Many requirements can be met with some combination of these capabilities...

### Give Star 🌟
**If you like this project and/or use it the please give it a Star 🌟 (c'mon it's free, and it'll help others find the project)!**

### [Buy me a Coffee ☕](https://www.buymeacoffee.com/cajuncoding)
*I'm happy to share with the community, but if you find this useful (e.g for professional use), and are so inclinded,
then I do love-me-some-coffee!*

<a href="https://www.buymeacoffee.com/cajuncoding" target="_blank">
<img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174">
</a> 

## Nuget Package
To use behaviors in your project, add the [iTextSharp.PdfHelpers NuGet package](https://www.nuget.org/packages/iTextSharp.PdfHelpers/) to your project.

## PdfHelpers.Resize
Lightweight Helper Library for performing tasks on Pdf files such as: scaling and resizing Pdf Documents and Pages of existing Pdf documents, converting Images into Pdf 
format, and merging Pdf Documents as well as merging Images into Existing Pdf documents.  It leverages iTextSharp for core work but offeres a easy wrapper for common 
resizing, conversion, and merging needs.  In addition, this library is intented to help simplify the task of rendering Pdf based outputs/reports and works best when 
combined with a template based Pdf Rendering process [such as the one here that allows use of Razor templates to render Pdf files](https://github.com/cajuncoding/XslFO.TestSolution).

It's as easy as . . .

#### Sample for Resizing Pdf Content:
```
byte[] pdfBytes = File.ReadAllBytes("...path to pdf...");
var targetSizeInfo = new PdfResizeInfo(PageSize.POSTCARD, PdfMarginSize.None);
byte[] scaledPdfBytes = PdfResizeHelper.ResizePdfPageSize(pdfBytes, targetSizeInfo, PdfScalingOptions.Default)
```


## PdfHelpers.Convert
Set of helpers for converting common things into Pdf (e.g. converting an Image into Pdf for mergeing/Combining into other Pdf documents.

#### Sample for Converting Image to Pdf Document:
```
var imageBytes = File.ReadAllBytes("...path to valid Image...");
var pageSizeInfo = new PdfResizeInfo(PageSize.POSTCARD, PdfMarginSize.None);
var pdfBytes = PdfConvertHelper.ConvertImageToPdf(imageBytes, pageSizeInfo, PdfScalingOptions.Default);
```   


## PdfHelpers.Merge
Set of helpers for merging Pdf documents together into one output Pdf. When combined with the Convert helper above it's 
easy to append Image files onto an existing Pdf).  

*NOTE: This uses the PdfSmartCopy underlying copier to merge the documents. Therfore, assuming they haven't been scaled/resized the annotations and dynamic
elements will be preserved.  This also enables the output document to be optimized without unnecessary duplicate references to embedded elemetns such as Fonts.*

#### Sample for Converting Image to Pdf Document:
```
var pageSizeInfo = new PdfResizeInfo(PageSize.POSTCARD, PdfMarginSize.None);

var pdfFileBytesList = new List<byte[]>() {
    File.ReadAllBytes("...path to pdf..."),
    PdfConvertHelper.ConvertImageToPdf("...path to valid Image 1...", pageSizeInfo),
    PdfConvertHelper.ConvertImageToPdf("...path to valid Image 2...", pageSizeInfo),
    PdfConvertHelper.ConvertImageToPdf("...path to valid Image 3...", pageSizeInfo),
};

var pdfBytes = PdfConvertHelper.MergePdfFiles(pdfFileBytesList, pageSizeInfo);
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
