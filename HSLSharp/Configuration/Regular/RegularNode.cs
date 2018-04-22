﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HslCommunication.Core;

namespace HSLSharp.Configuration
{
    public class RegularNode : NodeClass,IComparable<RegularNode>
    {
        public RegularNode( )
        {
            NodeType = NodeClassInfo.RegularNode;
        }


        /// <summary>
        /// 类型的代号，详细参见const数据
        /// </summary>
        public int RegularCode { get; set; }

        /// <summary>
        /// 类型的长度，对于string来说，就是字符串长度，其他的来说，就是数组长度
        /// </summary>
        public int TypeLength { get; set; }

        /// <summary>
        /// 数据位于字节数据的索引，对于bool变量来说，就是按照位的索引
        /// </summary>
        public int Index { get; set; }



        #region Translate

        public dynamic GetValue( byte[] data, IByteTransform byteTransform )
        {
            if (RegularCode == RegularNodeTypeItem.Int16.Code)
            {
                if (TypeLength == 1)
                {
                    return byteTransform.TransInt16( data, Index );
                }
                else
                {
                    return byteTransform.TransInt16( data, Index, TypeLength );
                }
            }
            else if (RegularCode == RegularNodeTypeItem.UInt16.Code)
            {
                if (TypeLength == 1)
                {
                    return byteTransform.TransUInt16( data, Index );
                }
                else
                {
                    return byteTransform.TransUInt16( data, Index, TypeLength );
                }
            }
            else if (RegularCode == RegularNodeTypeItem.Int32.Code)
            {
                if (TypeLength == 1)
                {
                    return byteTransform.TransInt32( data, Index );
                }
                else
                {
                    return byteTransform.TransInt32( data, Index, TypeLength );
                }
            }
            else if (RegularCode == RegularNodeTypeItem.UInt32.Code)
            {
                if (TypeLength == 1)
                {
                    return byteTransform.TransUInt32( data, Index );
                }
                else
                {
                    return byteTransform.TransUInt32( data, Index, TypeLength );
                }
            }
            else if (RegularCode == RegularNodeTypeItem.Int64.Code)
            {
                if (TypeLength == 1)
                {
                    return byteTransform.TransInt64( data, Index );
                }
                else
                {
                    return byteTransform.TransInt64( data, Index, TypeLength );
                }
            }
            else if (RegularCode == RegularNodeTypeItem.UInt64.Code)
            {
                if (TypeLength == 1)
                {
                    return byteTransform.TransUInt64( data, Index );
                }
                else
                {
                    return byteTransform.TransUInt64( data, Index, TypeLength );
                }
            }
            else if (RegularCode == RegularNodeTypeItem.Float.Code)
            {
                if (TypeLength == 1)
                {
                    return byteTransform.TransSingle( data, Index );
                }
                else
                {
                    return byteTransform.TransSingle( data, Index, TypeLength );
                }
            }
            else if (RegularCode == RegularNodeTypeItem.Double.Code)
            {
                if (TypeLength == 1)
                {
                    return byteTransform.TransDouble( data, Index );
                }
                else
                {
                    return byteTransform.TransDouble( data, Index, TypeLength );
                }
            }
            else if (RegularCode == RegularNodeTypeItem.StringAscii.Code)
            {
                return Encoding.ASCII.GetString( data, Index, TypeLength );
            }
            else
            {
                throw new Exception( "Not Supported Data Type" );
            }
        }


        #endregion

        #region IComparable Interface

        /// <summary>
        /// 实现了比较的接口，可以用来方便的排序
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo( RegularNode other )
        {
            return this.GetStartedByteIndex( ).CompareTo( other.GetStartedByteIndex( ) );
        }


        #endregion

        #region Public Method

        public int GetStartedByteIndex()
        {
            if(RegularCode == RegularNodeTypeItem.Bool.Code)
            {
                return Index / 8;
            }
            else
            {
                return Index;
            }
        }
        


        public int GetLengthByte( )
        {
            if (RegularCode == RegularNodeTypeItem.Bool.Code)
            {
                if ((Index + TypeLength) % 8 == 0)
                {
                    return (TypeLength) / 8 + GetStartedByteIndex( );
                }
                else
                {
                    return (TypeLength) / 8 + 1 + GetStartedByteIndex( );
                }
            }
            else if (RegularCode == RegularNodeTypeItem.StringAscii.Code ||
                RegularCode == RegularNodeTypeItem.StringUnicode.Code ||
                RegularCode == RegularNodeTypeItem.StringUtf8.Code)
            {
                return TypeLength + Index;
            }
            else if (RegularCode == RegularNodeTypeItem.UInt16.Code ||
                RegularCode == RegularNodeTypeItem.Int16.Code)
            {
                return TypeLength * 2 + Index;
            }
            else if (RegularCode == RegularNodeTypeItem.Int32.Code ||
                RegularCode == RegularNodeTypeItem.UInt32.Code ||
                RegularCode == RegularNodeTypeItem.Float.Code)
            {
                return TypeLength * 4 + Index;
            }
            else if (RegularCode == RegularNodeTypeItem.Int64.Code ||
                RegularCode == RegularNodeTypeItem.UInt64.Code ||
                RegularCode == RegularNodeTypeItem.Double.Code)
            {
                return TypeLength * 8 + Index;
            }
            else
            {
                return TypeLength + Index;
            }
        }

        #endregion
        
        #region Override

        /// <summary>
        /// 从XElement加载数据
        /// </summary>
        /// <param name="element"></param>
        public override void LoadByXmlElement( XElement element )
        {
            base.LoadByXmlElement( element );
            RegularCode = int.Parse( element.Attribute( "TypeCode" ).Value );
            TypeLength = int.Parse( element.Attribute( "TypeLength" ).Value );
            Index = int.Parse( element.Attribute( "Index" ).Value );
        }

        /// <summary>
        /// 转化Xml存储文件
        /// </summary>
        /// <returns></returns>
        public override XElement ToXmlElement( )
        {
            XElement element = new XElement( "RegularNode" );
            element.SetAttributeValue( "Name", Name );
            element.SetAttributeValue( "Description", Description );
            element.SetAttributeValue( "Index", Index );
            element.SetAttributeValue( "TypeCode", RegularCode );
            element.SetAttributeValue( "TypeLength", TypeLength );
            return element;
        }

        
        #endregion


    }
}
