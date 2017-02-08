### 服务器端插件开发

​	服务器端的插件建立和PC端的基本一致其（此处参考实例即可），由于服务器端是通过WebAPI的方式提供相关操作接口，所以需要引用相应的组件并创建`ApiController`接口类，其中dll的引用请参考示例程序进行依次添加。服务器端插件的结构相对比较简单，需要注意以下几点：

- 需要添加`WebApiConfig`类，直接拷贝就行
- 勾选并项目的XML输出文件：`bin\Debug\helpApi.xml`（选择项目右键菜单->点击属性->选择Build，即可找到XML document file）。
- 注意Controller的命名（XXXController）及目录结构（Controllers->V1->xxxController）