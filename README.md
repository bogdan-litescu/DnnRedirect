DNN Redirect by [DNN Sharp](http://www.dnnsharp.com)
===================================================

## Why is DNN Redirect FREE?


As we received many feature requests from the community we realized DNN Redirect had a huge limitation in terms of adding new Redirect Types. The 3 types it supports they are each in its own database table and being evaluated sequentially based on types (all Parameter Rules first, then all Referrer rules, and finally all Role Rules). This made it difficult to extend with new types or to manage and configure rules.


So we decided to start from scratch with a better architecture, and Redirect Toolkit was born. It has a generic interface for rules (so there's only one table with the possibility to prioritize rules of any kind). And in release 2.0 we added possibility for developers to write their own redirect types, operators or additional actions and hook them into Redirect Toolkit.

And now we feel that instead of throwing DNN Redirect away we should give it for FREE to the community, promoting Redirect Toolkit and our other modules altogether.
If you need more, make sure to:

1. [Read what Redirect Toolkit brings to the table](http://www.dnnsharp.com/dnn/modules/redirect/upgrade-benefits)
2. [Download Redirect Toolkit free trial](http://www.dnnsharp.com/dnn/modules/workflow-segementation/redirect-toolkit/download)
3. [Follow us on Twitter to stay connected](http://twitter.com/dnnsharp) 


## Installing DNN Redirect

You can use the installer package which can be downloaded from [DNN Redirect downloads page](http://www.dnnsharp.com/dnn/modules/workflow-segementation/redirect-toolkit/download).

This will take care to create the module definition and install the required tables. 
If you also need the source code, you can checkout this repository in the same folder.
Compiling the solution will put the assemblies in the website /bin folder.


<div style="float: right; text-align: right;">
  <a href="http://www.dnnsharp.com"><img src="http://static.dnnsharp.com/logo/dnnsharp-v2-100.png" title="DNN Sharp" /></a>&nbsp;&nbsp;
  <a href="http://www.dnnsharp.com/dnn/modules/redirect"><img src="http://static.dnnsharp.com/logo/dnn-modules/dnn-redirect-100t.png" title="DNN Redirect homepage" /></a>&nbsp;&nbsp;
  <a href="http://www.dnnsharp.com/dnn/modules/workflow-segementation/redirect-toolkit"><img src="http://static.dnnsharp.com/logo/dnn-modules/redirect-toolkit-100t.png" title="Redirect Toolkit homepage" /></a>
</div>
