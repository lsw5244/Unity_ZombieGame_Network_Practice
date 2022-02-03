﻿using System;
using UnityEngine;
using UnityEngine.UI;
// 생명체로서 동작할 게임 오브젝트들을 위한 뼈대를 제공
// 체력, 데미지 받아들이기, 사망 기능, 사망 이벤트를 제공
public class LivingEntity : MonoBehaviour, IDamageable
{
    public Slider HealthSlider; // 체력을 표시할 UI 슬라이더

    public float StartingHealth = 100f; // 시작 체력
    public float CurrentHealth {
        get { return _health; }
        protected set
        {
            _health = value;

            if(HealthSlider != null)
            {
                HealthSlider.value = _health;
            }
        }
    } // 현재 체력
    public bool IsDead { get; protected set; } // 사망 상태
    public event Action OnDeath; // 사망시 발동할 이벤트     // Action은 반환형 void 반환형 x

    private float _health;
    // 생명체가 활성화될때 상태를 리셋
    protected virtual void OnEnable()
    {
        if(HealthSlider != null)
        {
            HealthSlider.gameObject.SetActive(true);
            HealthSlider.maxValue = StartingHealth;
        }
        // 사망하지 않은 상태로 시작
        IsDead = false;
        // 체력을 시작 체력으로 초기화
        CurrentHealth = StartingHealth;
    }

    // 데미지를 입는 기능
    public virtual void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // 데미지만큼 체력 감소
        CurrentHealth -= damage;

        // 체력이 0 이하 && 아직 죽지 않았다면 사망 처리 실행
        if (CurrentHealth <= 0 && IsDead == false)
        {
            Die();
        }
    }

    // 체력을 회복하는 기능
    public virtual void RestoreHealth(float newHealth)
    {
        if (IsDead)
        {
            // 이미 사망한 경우 체력을 회복할 수 없음
            return;
        }

        // 체력 추가
        CurrentHealth += newHealth;
    }

    // 사망 처리
    public virtual void Die()
    {
        // onDeath 이벤트에 등록된 메서드가 있다면 실행
        OnDeath?.Invoke();

        // 사망 상태를 참으로 변경
        IsDead = true;

        HealthSlider?.gameObject.SetActive(false);
    }
}