   M S B A M L   `   `   `  ����L  HSecureFileManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:SecureFileManagerSecureFileManager  5   	   "    SecureFileManager.WindowOpen   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:SecureFileManager     5   	   -   5        $	��360��5   	   $	��425��5	   	     � 
ResizeMode$  NoResize=�5
   	    � Icon$ icon.ico��5   	    � Title$ open a file��5   	   .��5      � $��0q�6   .+�5   
   � $��46��6   $��Bottom=�6   ��5      �� 5       �� Color# � ��6"   .+�5      �� -   5         ��Name$ lblError��5      $�� ��6&   $��Right=�61   $��
0,10,10,10q�6M   ���	#FFC100006a   �� 5      -   5         ��Name$ btnOpen��5      $
��Open��6&   $
��Left=�65   $��
10,13,0,13q�6P   $��82��6d   {�.   �� 	IsDefault . 6�   �� 5      -   5        $ btnDeleteFile��5      $��Delete��6,   $
��Left=�6=   $��102,13,0,13q�6X   $��82��6m   {�.  6�   5      �� 5   
   $��	Password*��6   $��64��6&   $��10,206,10,0q�62   $	��Top=�6G   .��5      � 
�������.+�5      G� -   5         G�Name$ 	pwbPasswd��5      $��
10,10,10,0q�61   $	��Top=�6E   $��Center=�6]   $��20��6   5      5      �� 5   
   $��Database��6   $	��191��6%   $��
10,10,10,0q�62   $	��Top=�6F   .��5      � 
�������.+�5       �� $��File:��6   $
��Left=�6(   $��	10,41,0,0q�6C   $	��Top=�6V   �� 5!      -   5         ��Name$ txtFilePath��5!      $��20��6/   $��107,46,45,0q�6;   $	��Top=�6P   $��Center=�6h   6�   �� 5"      -   5        $ btnFileDialog��5"      $	��...��60   $��Right=�6>   $��	0,46,10,0q�6Z   $	��Top=�6m   $��30��6�   $��20��6�   �� 5#      $��Length:��6   $
��Left=�6*   $��	10,72,0,0q�6E   $	��Top=�6X   �� 5$      $��Creation date:��6   $
��Left=�61   $��
10,103,0,0q�6L   $	��Top=�6`   �� 5%      $��
last edit:��6   $
��Left=�6-   $��
10,134,0,0q�6H   $	��Top=�6\   �� 5&      -   5        $ 	lblLength��5&      $	��---��6+   $
��Left=�69   $��
107,72,0,0q�6T   $	��Top=�6h   �� 5'      -	   5        $ lblCreationDate��5'      $	��---��61   $
��Left=�6?   $��107,103,0,0q�6Z   $	��Top=�6o   �� 5(      -
   5        $ lblLastEdit��5(      $	��---��6-   $
��Left=�6;   $��107,134,0,0q�6V   $	��Top=�6k   �� 5)      $��Mode:��6   $
��Left=�6(   $��	10,10,0,0q�6C   $	��Top=�6V   	� 5*      -   5        	 	�Name$	 	rdbCreate��5*      $��Create a new file��61   $
��Left=�6M   $��
242,16,0,0q�6h   $	��Top=�6|   $��Center=�6�   6�   	� 5+      -   5        $	 rdbOpen��5+      
 d� 	IsChecked$

 TrueY�6/   $��Open existing file��6@   $
��Left=�6]   $��
103,16,0,0q�6x   $	��Top=�6�   $��Center=�6�   6�   5,      5-      5/      50      