using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;

namespace API.Services
{
    public class BaseService<TEntity, TDTO> : IBaseService<TEntity, TDTO> 
        where TEntity : class 
        where TDTO : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual async Task<TDTO> Create(TDTO dto)
        {

            var entity = _mapper.Map<TEntity>(dto);

            await _unitOfWork.Repository<TEntity>().AddAsync(entity);
            await _unitOfWork.SaveAsync();
            
            return _mapper.Map<TDTO>(entity);
        }

        public virtual async Task<TDTO> Update(int id, TDTO dto)
        {
            var existingEntity = await _unitOfWork.Repository<TEntity>().GetByIdAsync(id);
            if (existingEntity == null)
                throw new KeyNotFoundException($"Entity with ID {id} not found");
            
            _mapper.Map(dto, existingEntity);
            
            _unitOfWork.Repository<TEntity>().Update(existingEntity);
            await _unitOfWork.SaveAsync();
            
            return _mapper.Map<TDTO>(existingEntity);
        }

        public virtual async Task<bool> Delete(int id)
        {
            var entity = await _unitOfWork.Repository<TEntity>().GetByIdAsync(id);
            if (entity == null)
                return false;
            
            _unitOfWork.Repository<TEntity>().Delete(entity);
            
            return await _unitOfWork.SaveAsync();
        }

        public virtual async Task<TDTO> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<TEntity>().GetByIdAsync(id);
            if (entity == null)
                return null;
            
            return _mapper.Map<TDTO>(entity);
        }

        public virtual async Task<ICollection<TDTO>> GetAllAsync()
        {
            var entities = await _unitOfWork.Repository<TEntity>().GetAllAsync();
            
            return _mapper.Map<ICollection<TDTO>>(entities);
        }
        
        public virtual async Task<ICollection<TDTO>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await _unitOfWork.Repository<TEntity>().FindAsync(predicate);   
            return _mapper.Map<ICollection<TDTO>>(entities);
        }
    }
}