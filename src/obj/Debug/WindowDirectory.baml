   M S B A M L   `   `   `  ����L  HSecureFileManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:SecureFileManagerSecureFileManager  5   	   '    !SecureFileManager.WindowDirectory   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:SecureFileManager     5   	   -   5          � Title$  	Directory��5   	   $	��240��6   $	��400��6(    � 
ResizeMode$ NoResize=�64   .��5	      � .+�5
   
   �� $��Name:��6   $
��Left=�6    $��	10,10,0,0q�6;   $	��Top=�6N   �� 5   
   -   5         ��Name$ 
txtDirName��5      $��20��6&   $��
95,15,20,0q�62   $	��Top=�6F   $��Center=�6^    �� 	MaxLength$ 50��6�    �� MaxLines$ 1��6�   �� 5   
   $��Description:��6   $
��Left=�6'   $��	10,72,0,0q�6B   $	��Top=�6U   �� 5   
   -   5        $ txtDirDescr��5      $��
95,75,20,0q�6'   $	��Top=�6;   $	 100��6S   $ 3��6c   $��34��6p    �� TextWrapping$
 Wrap=�6|   �� 5   
   $��Label:��6   $
��Left=�6!   $��	10,41,0,0q�6<   $	��Top=�6O   �� 5   
   -   5         ��Name$ cmbLabel��5      $
��Left=�6%   $��	95,45,0,0q�6@   $	��Top=�6S   $��72��6k   $��20��6v   $	��5,1q�6�   
���   � �� SelectedIndex$ -1��6�   .@�5      �� $��black��6   
���   �
�������6>   �� 5      $	��red��6   
���  ��6)   �� 5      $
��blue��6   
����  �6*   �� 5      $��orange��6   
��� ���6,   �� 5      $��green��6   
��� � �6+   5      � 5   
   $��0,0,0,1q�6   $��46��6    $��Bottom=�6,   ��5      �� 5       �� Color# � ��6"   .+�5      �� -   5        	 ��Name$	 btnSave��5      $
��Save��6&   $
��Left=�65   $��
10,13,0,13q�6P   $��82��6d   6o   �� 5      -   5        $	 	btnRemove��5      $��Delete��6(   $
��Left=�69   $��102,13,-23,13q�6T   $��82��6k   6v   5      5      5      