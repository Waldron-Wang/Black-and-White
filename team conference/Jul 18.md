# 任务安排
考虑到大家的时间安排，本次任务主要为美工和代码  
分别由梁和我负责  
其次是陈的任务，主要是探索和学习  
## 美工
### tileset
本次任务更加具体  
![图一](https://github.com/Waldron-Wang/Black-and-White/Assets/Pixel Art Snowy Forest/PNG/tileset.png)
![图二](https://github.com/Waldron-Wang/Black-and-White/Assets/Free Pixel Art Forest/PNG/Hills Layer 05.png)
制作一个和图一一模一样的tileset，仅把雪地替换成图二的草坪  
并用图二最下面一点点的泥土（仔细看，草下面是有土的）的颜色替换掉图一的泥土的颜色  
其次图一泥土边缘是明显的冻土，看看能不能稍作修改让它看起来不这么干  
但如果对冻土的修改没什么信心，可以不改，若作出修改，请保留一份未修改的副本  
提示：  
修改雪地时直接将图二的草坪复制过去，将雪地盖住，然后对衔接部分稍作修改使其看起来更自然  
并不需要对草坪本身作出修改  
其最终效果大概如图三（图三是另外一种草坪，且只完成了图一的部分tile）  
![图三](https://github.com/Waldron-Wang/Black-and-White/2D URP animation/Assets/environment/Level 3/L3 tileset.png)
### 背景群山
![图四](https://github.com/Waldron-Wang/Black-and-White/Assets/Free Pixel Art Forest/PNG/Hills Layer 01.png)
山的形状过于奇怪，将其改为正常的形状，写实一点  
其他所有的都不用变（颜色，花纹）  
但是山后面的背景无所谓，可以直接去掉也可以留着  
完成后再加一个新图层，将新图层放在原图层下方，复制原图层的内容  
然后给新图层加一层滤镜，使其颜色较原图层略浅，作为更远处的山  
## 代码
继续重构角色控制脚本
## 陈的工作
探索如何设计和实现敌人
对敌人的机制设计有基本的思路，可以从以下几点出发:  
敌人有哪几种不同的状态，例如游荡（没发现玩家），追踪（发现玩家后），攻击  
思考如何通过unity的工具和代码实现敌人的状态机和每个状态的具体逻辑  
相关问题可以问chatgpt或者YouTube上的相关资源（prompt：如何在unity中实现...）  
一般chatgpt会给你几种常见的方法，然后再到YouTube上搜索相关资源（YouTube上这方面的资源还是相当丰富的）  
可以尝试自己实现敌人的代码