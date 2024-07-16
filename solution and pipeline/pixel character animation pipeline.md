# 功能
实现3D动画直接渲染成2D像素动画，避免手绘，实现了大批量生产动画
# pipeline
## 3d model
model(只导出面和骨骼，其他物体无需导入，会引入很多不需要的文件)——到mixamo上自动生成rig（一定要调试好，生成最佳rig，保持原来姿势）
——在maya中首先重新skin（重新skin前尽量不动人物，防止模型变形）
——最后define rig并生成rig control——选择mesh和rig导入unity——提取材质，完成材质设置  
## export into unity
### avatar模型：
模型rig设置，创建avatar——应用动画，并删除动画的root x z（使动画在原地播放而不产生位移）
——创建timeline并record出新的动画文件——替换动画文件并导入2D pixel animation
### 非avatar模型：
（如果源文件不是in place要使其in place）导入3Danimation——复制到2D pixel animation——
## render into 2D pixel
### 调整导入设置
调整模型scale factor——将动画类型改为legacy
——将material改为使用外部材质
（在该unity项目中有单独的材质文件夹，使用外部材质就是使用该材质文件夹）
### 导出2d像素动画
设置场景中的capturer要capture的对象、动画、大小和帧率
按capture按钮生成sprite sheet——将sprite sheet导入2D URP的texture文件夹